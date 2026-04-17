# Azure Functions Generation Workflow

This guide shows how a developer starts with a requirements file and produces a generation-ready prompt for Azure Functions code generation.

## Step 0: Pull the repository

Clone the repository and move into the repo root:

```powershell
git clone https://github.com/nafshar2026/sdlc.git
cd sdlc
```

All commands below assume you are running from the repository root.

## What this process does

1. Reads a requirements file.
2. Auto-selects the relevant SKILL files using keyword matching.
3. Merges those skills into one context file.
4. Builds a final generation prompt file.
5. Sends that prompt to a code generation tool.
6. Writes generated code to the output folder defined in the requirements.

## Inputs

Primary input files:
- `artifacts/sample-requirements.txt` for a working example.
- `artifacts/requirements-template.txt` for creating a new requirements file.

Skill system files:
- `skills/azure-functions-generation/LOAD-ORDER.md`
- `skills/azure-functions-generation/SKILL-MAP.json`
- `skills/azure-functions-generation/core/`
- `skills/azure-functions-generation/engineering/`
- `skills/azure-functions-generation/quality/`
- `skills/azure-functions-generation/delivery/`

Automation scripts:
- `skills/azure-functions-generation/scripts/load_skills.ps1`
- `skills/azure-functions-generation/scripts/load_skills.py`
- `skills/azure-functions-generation/scripts/prepare_generation_prompt.ps1`

## Step 1: Create or update a requirements file

Start from `artifacts/requirements-template.txt` and replace placeholders with real project details.

In the Output requirements section, each team should set its own project folder name, for example:
- `./generated/customer-docs-func`
- `./generated/dealer-docs-func`

For a quick trial, use:
- `artifacts/sample-requirements.txt`

The sample generated project already committed in this repo is:
- `generated/sample-azure-functions`

The README inside that sample project:
- `generated/sample-azure-functions/README.md`

is generated output documentation for that sample project. It is not the primary workflow documentation for this repository.

## Step 2: Auto-select and merge the relevant skills

PowerShell:

```powershell
./skills/azure-functions-generation/scripts/load_skills.ps1 -RequirementsFile ./artifacts/sample-requirements.txt -Out ./artifacts/skills-context.auto.md
```

This generates a merged skills context file:
- `artifacts/skills-context.auto.md`

## Step 3: Build the final generation prompt

PowerShell:

```powershell
./skills/azure-functions-generation/scripts/prepare_generation_prompt.ps1 -RequirementsFile ./artifacts/sample-requirements.txt -Out ./artifacts/generation-prompt.md
```

This generates:
- `artifacts/generation-prompt.md`

That prompt file contains:
- the original requirements
- the auto-selected skills context
- generation instructions
- expected output path
- expected solution folder structure

This is the handoff artifact for the code generation step.

Do not manually edit this generated prompt for normal runs. It is intended to be consumed as-is.
If repeated customizations are required, update the requirements template, skills files, or `prepare_generation_prompt.ps1` so changes are systematic.

## Step 4: Send the prompt to the code generation tool

Use the contents of `artifacts/generation-prompt.md` as the input to your code generation step.

Examples:
- paste it into an internal LLM-based code generation workflow
- pass it to an automation job that invokes a coding agent
- store it as a build artifact for a downstream generation stage

At this step, the developer is no longer editing the requirements or skills files. The input to generation is the prompt file created in Step 3.

## Step 5: Find the generated code

The generated code should be written to the path specified in the requirements file.

In the current sample requirements, that path is:
- `./generated/sample-azure-functions`

Expected generated solution layout:

```text
./generated/sample-azure-functions/
  src\
    Functions\
    Application\
    Domain\
    Infrastructure\
  tests\
    UnitTests\
    IntegrationTests\
  Directory.Build.props
  Directory.Build.targets
  README.md
```

The exact file names under each folder depend on the requirements and the code generator, but the solution should follow the structure above.

Each generated project should also include its own generated `README.md` describing setup, configuration, local execution, and test steps.

For team-specific runs, this folder name changes based on the requirements file. Example:
- `./generated/customer-docs-func`
- `./generated/dealer-docs-func`

## Step 6: Validate the output

After generation:
1. Confirm the target folder exists.
2. Confirm the `src` and `tests` folders were created.
3. Confirm configuration files and README were generated.
4. Run tests.
5. Review the generated code against the original requirements file.

## Example end-to-end run

```powershell
git clone https://github.com/nafshar2026/sdlc.git
cd sdlc
./skills/azure-functions-generation/scripts/load_skills.ps1 -RequirementsFile ./artifacts/sample-requirements.txt -Out ./artifacts/skills-context.auto.md
./skills/azure-functions-generation/scripts/prepare_generation_prompt.ps1 -RequirementsFile ./artifacts/sample-requirements.txt -Out ./artifacts/generation-prompt.md
```

Then use:
- `artifacts/generation-prompt.md`

as the direct input to your code generation tool.

## Notes

- `LOAD-ORDER.md` is only a manifest. It does not auto-load skills by itself.
- `SKILL-MAP.json` controls automatic skill selection.
- If your team writes requirements using different terminology, update `SKILL-MAP.json` so the correct skills are selected.
- This repository prepares the generation context and prompt. The actual file creation under the target output path is performed by the downstream code generator.
