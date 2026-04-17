param(
    [string]$Manifest = "../LOAD-ORDER.md",
    [string[]]$Skills,
    [string]$RequirementsFile,
    [string]$RequirementsText,
    [string]$SkillMapFile = "../SKILL-MAP.json",
    [string]$Out
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$manifestPath = (Resolve-Path -LiteralPath (Join-Path $scriptDir $Manifest) -ErrorAction Stop).Path
$manifestDir = Split-Path -Parent $manifestPath
$skillMapPath = (Resolve-Path -LiteralPath (Join-Path $scriptDir $SkillMapFile) -ErrorAction Stop).Path

$entries = @()
foreach ($line in Get-Content -LiteralPath $manifestPath) {
    if ($line -match '^\s*\d+\.\s+(.+\.md)\s*$') {
        $relative = $Matches[1].Trim()
        $fullPath = (Resolve-Path -LiteralPath (Join-Path $manifestDir $relative) -ErrorAction Stop).Path
        $entries += $fullPath
    }
}

if ($entries.Count -eq 0) {
    throw "No skill entries found in manifest: $manifestPath"
}

$selectedIds = @()
if ($null -ne $Skills -and $Skills.Count -gt 0) {
    foreach ($chunk in $Skills) {
        if ([string]::IsNullOrWhiteSpace($chunk)) {
            continue
        }

        foreach ($part in ($chunk -split '[,\s]+')) {
            $value = $part.Trim()
            if (-not [string]::IsNullOrWhiteSpace($value)) {
                $selectedIds += $value.PadLeft(2, '0')
            }
        }
    }
}

$skillMapObject = Get-Content -LiteralPath $skillMapPath -Raw | ConvertFrom-Json
$defaultSkills = @($skillMapObject.PSObject.Properties['defaultSkills'].Value)
$skillsMetadata = @($skillMapObject.PSObject.Properties['skills'].Value)

foreach ($defaultSkill in $defaultSkills) {
    $selectedIds += ([string]$defaultSkill).PadLeft(2, '0')
}

$requirementsContent = $RequirementsText
if (-not [string]::IsNullOrWhiteSpace($RequirementsFile)) {
    $requirementsPath = (Resolve-Path -LiteralPath $RequirementsFile -ErrorAction Stop).Path
    $requirementsContent = Get-Content -LiteralPath $requirementsPath -Raw
}

if (-not [string]::IsNullOrWhiteSpace($requirementsContent)) {
    $requirementsLower = $requirementsContent.ToLowerInvariant()
    foreach ($skill in $skillsMetadata) {
        foreach ($keyword in $skill.keywords) {
            if ($requirementsLower.Contains(([string]$keyword).ToLowerInvariant())) {
                $selectedIds += ([string]$skill.id).PadLeft(2, '0')
                break
            }
        }
    }
}

$selectedIds = $selectedIds | Sort-Object -Unique

$selected = @()
foreach ($entry in $entries) {
    $name = [System.IO.Path]::GetFileName($entry)
    if ($selectedIds.Count -eq 0) {
        $selected += $entry
        continue
    }

    if ($name -match 'SKILL-(\d+)-') {
        $skillId = $Matches[1].PadLeft(2, '0')
        if ($selectedIds -contains $skillId) {
            $selected += $entry
        }
    }
}

if ($selected.Count -eq 0) {
    throw "No matching skills selected."
}

$builder = [System.Text.StringBuilder]::new()
$null = $builder.AppendLine("# Loaded Skill Context")
$null = $builder.AppendLine()
$manifestLabel = $manifestPath
$cwdPath = (Get-Location).Path
if ($manifestPath.StartsWith($cwdPath, [System.StringComparison]::OrdinalIgnoreCase)) {
    $manifestLabel = $manifestPath.Substring($cwdPath.Length).TrimStart('\\') -replace '\\', '/'
    if (-not [string]::IsNullOrWhiteSpace($manifestLabel)) {
        $manifestLabel = './' + $manifestLabel
    }
}
$null = $builder.AppendLine("Manifest: $manifestLabel")
$null = $builder.AppendLine("Loaded count: $($selected.Count)")
$null = $builder.AppendLine()

foreach ($file in $selected) {
    $name = [System.IO.Path]::GetFileName($file)
    $null = $builder.AppendLine("## BEGIN $name")
    $null = $builder.AppendLine()
    $null = $builder.AppendLine((Get-Content -LiteralPath $file -Raw).TrimEnd())
    $null = $builder.AppendLine()
    $null = $builder.AppendLine("## END $name")
    $null = $builder.AppendLine()
}

$payload = $builder.ToString()

if ([string]::IsNullOrWhiteSpace($Out)) {
    Write-Output $payload
}
else {
    if ([System.IO.Path]::IsPathRooted($Out)) {
        $outPath = [System.IO.Path]::GetFullPath($Out)
    }
    else {
        $outPath = [System.IO.Path]::GetFullPath((Join-Path (Get-Location) $Out))
    }
    $outDir = Split-Path -Parent $outPath
    if (-not [string]::IsNullOrWhiteSpace($outDir) -and -not (Test-Path -LiteralPath $outDir)) {
        $null = New-Item -ItemType Directory -Path $outDir -Force
    }

    [System.IO.File]::WriteAllText($outPath, $payload, [System.Text.Encoding]::UTF8)
    Write-Output "Wrote merged skills payload: $outPath"
}
