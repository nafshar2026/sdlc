# Azure Functions Skills Pack

This folder contains modular instruction files for generating a .NET 10 Azure Functions solution.

## Load Order
- ./LOAD-ORDER.md

## Skill Selection
- Automatic skill selection is driven by ./SKILL-MAP.json.
- The loader scripts read requirements text, match keywords, and select the relevant SKILL files.
- Baseline default skills are always included unless you change ./SKILL-MAP.json.
- Manual skill ids can still be supplied and are unioned with the auto-selected set.

## Loader Scripts
- ./scripts/load_skills.ps1
- ./scripts/load_skills.py
- ./scripts/prepare_generation_prompt.ps1
- ./scripts/USAGE.md

## Workflow Docs
- ./docs/STEP-BY-STEP.md
- ../../artifacts/requirements-template.txt

## PowerShell Examples

Load all skills:

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -Out ./artifacts/skills-context.md
```

Load selected skills manually:

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -Skills 01,03,05 -Out ./artifacts/skills-context.md
```

Auto-select skills from a requirements file:

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -RequirementsFile ./artifacts/sample-requirements.txt -Out ./artifacts/skills-context.auto.md
```

Auto-select skills from inline requirements text:

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -RequirementsText "Generate Azure Functions with FluentValidation, Polly retries, tests, and SonarQube coverage" -Out ./artifacts/skills-context.auto.md
```

## Python Examples

Load all skills:

```bash
python ./skills/azure-functions-generation/scripts/load_skills.py --out ./artifacts/skills-context.md
```

Load selected skills manually:

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
1. Save incoming requirements to a text file or pass them inline.
2. Run one of the loader scripts to build a merged skills context file.
3. Optionally run ./scripts/prepare_generation_prompt.ps1 to build a single generation prompt file.
4. Send both the raw requirements and the merged skills context, or the single generation prompt file, to the code generation step.
5. Generate the Azure Functions solution.
6. Run tests and validation.

## Important Notes
- LOAD-ORDER.md is only a manifest and does not load files by itself.
- Skills are loaded only when your automation explicitly runs one of the loader scripts.
- Auto-selection is heuristic and keyword-based; update ./SKILL-MAP.json when your requirement language changes.

## Core
- ./core/SKILL-01-scope.md
- ./core/SKILL-02-tech-stack.md
- ./core/SKILL-03-architecture-rules.md
- ./core/SKILL-04-configuration.md

## Engineering
- ./engineering/SKILL-05-validation.md
- ./engineering/SKILL-06-retry-resiliency.md

## Quality
- ./quality/SKILL-07-testing.md
- ./quality/SKILL-08-coverage-ci.md
- ./quality/SKILL-09-security-standards.md

## Delivery
- ./delivery/SKILL-10-output-target.md
