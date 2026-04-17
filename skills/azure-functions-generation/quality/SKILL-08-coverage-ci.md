# Coverage and CI Standards

- Use coverlet.msbuild, not coverlet.collector, for SonarQube-compatible coverage output.
- Remove any coverlet.collector references from test projects.
- Do not use XPlat Code Coverage collector commands that emit binary .coverage output.
