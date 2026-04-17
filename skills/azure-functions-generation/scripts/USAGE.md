# Minimal Loader Usage

Use these scripts in automation to build one merged context payload from selected SKILL files.

Automatic selection is supported through keyword matching against [../SKILL-MAP.json](../SKILL-MAP.json).

## PowerShell

Load all skills and write to file:

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -Out ./artifacts/skills-context.md
```

Load specific skills only (example: scope, architecture, validation):

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -Skills 01,03,05 -Out ./artifacts/skills-context.md
```

Quoted form (also valid):

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -Skills "01,03,05" -Out ./artifacts/skills-context.md
```

Auto-select skills from a requirements file:

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -RequirementsFile ./artifacts/sample-requirements.txt -Out ./artifacts/skills-context.auto.md
```

Auto-select skills from inline requirements text:

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -RequirementsText "Generate Azure Functions with FluentValidation, Polly retries, tests, and SonarQube coverage" -Out ./artifacts/skills-context.auto.md
```

## Python

Load all skills and write to file:

```bash
python ./skills/azure-functions-generation/scripts/load_skills.py --out ./artifacts/skills-context.md
```

Load specific skills only:

```bash
python ./skills/azure-functions-generation/scripts/load_skills.py --skills 01,03,05 --out ./artifacts/skills-context.md
```

Auto-select skills from a requirements file:

```bash
python ./skills/azure-functions-generation/scripts/load_skills.py --requirements-file ./artifacts/sample-requirements.txt --out ./artifacts/skills-context.auto.md
```

Auto-select skills from inline requirements text:

```bash
python ./skills/azure-functions-generation/scripts/load_skills.py --requirements-text "Generate Azure Functions with FluentValidation, Polly retries, tests, and SonarQube coverage" --out ./artifacts/skills-context.auto.md
```

## Typical Automation Flow

1. Provide raw feature requirements to the loader.
2. Let the loader auto-select skills from the mapping file, optionally unioned with manual skill ids.
3. Run loader script to produce merged context markdown.
4. Send requirements plus merged skill context to your code generation step.
5. Generate Azure Functions code.
6. Run tests and validation.

## Important Behavior

- LOAD-ORDER.md is a manifest only.
- Skills are loaded only when your pipeline explicitly runs one of these scripts.
- Selecting skill ids keeps context smaller and more focused.
- Auto-selection is heuristic and keyword-based; update SKILL-MAP.json when your requirement language changes.
