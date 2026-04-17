# Azure Functions Skills Pack

This folder contains modular instruction files for generating a .NET 10 Azure Functions solution.

## Start Here

Pull the repository first:

```powershell
git clone https://github.com/nafshar2026/sdlc.git
cd sdlc
```

Before running generation, read the enterprise standards pack:
- ./standards/README.md
- ./standards/AI-AGENT-GUARDRAILS.md
- ./standards/SECURITY.md

Then follow this end-to-end flow:
1. Create a new requirements file from `artifacts/requirements-template.txt`.
2. Set the output folder in that requirements file to a team-specific path such as `./generated/customer-docs-func`.
3. Run `./skills/azure-functions-generation/scripts/load_skills.ps1` to auto-select and merge the relevant skills.
4. Run `./skills/azure-functions-generation/scripts/prepare_generation_prompt.ps1` to build the final generation prompt.
5. Send the generated prompt file to your code generation tool or coding agent.
6. Find the generated Azure Functions solution in the output folder named in the requirements file.
7. Build and test the generated solution.

For the full detailed walkthrough, see `./docs/STEP-BY-STEP.md`.

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
- ../../artifacts/sample-requirements.txt

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
1. Pull this repo locally with `git clone`.
2. Create a project-specific requirements file from `artifacts/requirements-template.txt`.
3. Set the target output folder name in that requirements file.
4. Run one of the loader scripts to build a merged skills context file.
5. Run `./scripts/prepare_generation_prompt.ps1` to produce the final generation prompt.
6. Use the generated prompt as-is in the code generation step (do not manually edit it for normal runs).
7. Generate the Azure Functions solution in the folder named in the requirements file.
8. Run tests and validation on the generated solution.

## Important Notes
- Apply standards in this order:
	1. `./standards/AI-AGENT-GUARDRAILS.md`
	2. Other files in `./standards/`
	3. Azure Functions skills files in this folder
- LOAD-ORDER.md is only a manifest and does not load files by itself.
- Skills are loaded only when your automation explicitly runs one of the loader scripts.
- Auto-selection is heuristic and keyword-based; update ./SKILL-MAP.json when your requirement language changes.
- The generation prompt is an auto-generated artifact. Keep it unmodified for normal runs.
- If recurring prompt adjustments are needed, update the requirements template, skill files, or prompt builder script instead of hand-editing each generated prompt.
- `generated/sample-azure-functions` is a committed sample output for reference.
- `generated/sample-azure-functions/README.md` is generated project documentation produced as part of the sample output.
- Future team-generated projects should also contain their own generated `README.md` in the target output folder.
- Future team-generated projects should use their own folder names under `./generated/`.

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
