param(
    [Parameter(Mandatory = $true)]
    [string]$RequirementsFile,
    [string]$Out = "./artifacts/generation-prompt.md",
    [string]$SkillsContextOut = "./artifacts/skills-context.auto.md"
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Resolve-RepoPath {
    param(
        [string]$BasePath,
        [string]$InputPath
    )

    if ([System.IO.Path]::IsPathRooted($InputPath)) {
        return [System.IO.Path]::GetFullPath($InputPath)
    }

    $normalized = $InputPath -replace '^[.][\\/]', ''
    return [System.IO.Path]::GetFullPath((Join-Path $BasePath $normalized))
}

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = (Resolve-Path -LiteralPath (Join-Path $scriptDir "../../..") -ErrorAction Stop).Path
$loaderScript = (Resolve-Path -LiteralPath (Join-Path $scriptDir "load_skills.ps1") -ErrorAction Stop).Path
$requirementsPath = (Resolve-Path -LiteralPath $RequirementsFile -ErrorAction Stop).Path
$skillsContextPath = Resolve-RepoPath -BasePath $repoRoot -InputPath $SkillsContextOut
$outPath = Resolve-RepoPath -BasePath $repoRoot -InputPath $Out

$skillsContextDir = Split-Path -Parent $skillsContextPath
$outDir = Split-Path -Parent $outPath
if (-not (Test-Path -LiteralPath $skillsContextDir)) {
    $null = New-Item -ItemType Directory -Path $skillsContextDir -Force
}
if (-not (Test-Path -LiteralPath $outDir)) {
    $null = New-Item -ItemType Directory -Path $outDir -Force
}

& $loaderScript -RequirementsFile $requirementsPath -Out $skillsContextPath | Out-Null

$requirementsText = Get-Content -LiteralPath $requirementsPath -Raw
$skillsContextText = Get-Content -LiteralPath $skillsContextPath -Raw

$outputLocation = "Not found in requirements file"
foreach ($line in ($requirementsText -split "`r?`n")) {
    if ($line -match '^-\s*Generate the solution under\s+(.+?)\.?$') {
        $outputLocation = $Matches[1].Trim()
        break
    }
}

$builder = [System.Text.StringBuilder]::new()
$null = $builder.AppendLine("# Azure Functions Code Generation Prompt")
$null = $builder.AppendLine()
$null = $builder.AppendLine("Use the following requirements and loaded skill context to generate a complete .NET 10 Azure Functions solution.")
$null = $builder.AppendLine()
$null = $builder.AppendLine("## Output Location")
$null = $builder.AppendLine($outputLocation)
$null = $builder.AppendLine()
$null = $builder.AppendLine("## Expected Solution Structure")
$null = $builder.AppendLine('```text')
$null = $builder.AppendLine($outputLocation + '\')
$null = $builder.AppendLine('  src\')
$null = $builder.AppendLine('    Functions\')
$null = $builder.AppendLine('    Application\')
$null = $builder.AppendLine('    Domain\')
$null = $builder.AppendLine('    Infrastructure\')
$null = $builder.AppendLine('  tests\')
$null = $builder.AppendLine('    UnitTests\')
$null = $builder.AppendLine('    IntegrationTests\')
$null = $builder.AppendLine('  Directory.Build.props')
$null = $builder.AppendLine('  Directory.Build.targets')
$null = $builder.AppendLine('  README.md')
$null = $builder.AppendLine('```')
$null = $builder.AppendLine()
$null = $builder.AppendLine("## Requirements")
$null = $builder.AppendLine()
$null = $builder.AppendLine($requirementsText.TrimEnd())
$null = $builder.AppendLine()
$null = $builder.AppendLine("## Loaded Skill Context")
$null = $builder.AppendLine()
$null = $builder.AppendLine($skillsContextText.TrimEnd())
$null = $builder.AppendLine()
$null = $builder.AppendLine("## Generation Instructions")
$null = $builder.AppendLine()
$null = $builder.AppendLine("- Generate full source code, tests, configuration, and solution files.")
$null = $builder.AppendLine("- Write the generated solution to the output location shown above.")
$null = $builder.AppendLine("- Follow the requirements section first and use the loaded skill context as implementation guidance.")
$null = $builder.AppendLine("- Keep the trigger layer thin and place business logic in the appropriate application and infrastructure layers.")
$null = $builder.AppendLine("- Include README documentation for setup, local execution, and test execution.")

[System.IO.File]::WriteAllText($outPath, $builder.ToString(), [System.Text.Encoding]::UTF8)
Write-Output "Wrote generation prompt: $outPath"
