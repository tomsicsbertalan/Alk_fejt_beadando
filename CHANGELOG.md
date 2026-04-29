# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased] - 2026-04-29

- Security: upgraded `uuid` from `^13.0.0` to `^14.0.0` to address the advisory
  (GHSA-w5hq-g745-h8pq). This removed the reported moderate vulnerability.
- Performed automated and manual remediation steps in `source/WebUI`:
  - `npm audit`, `npm audit fix` (non-force)
  - `npm update`
  - `npm install`
- Tests: ran `npm test` in `source/WebUI` — all unit tests passed (14/14).
- Files changed:
  - `source/WebUI/package.json` (bumped `uuid` to `^14.0.0`)
- Compatibility notes:
  - `uuid` v14 is ESM-first; this project imports `v4` using
    `import { v4 as uuidv4 } from 'uuid'`, which remains compatible.
  - If any consumer uses CommonJS `require('uuid')`, migration to ESM or
    alternative interop may be necessary.

## Next steps

- Commit these changes and run CI to validate upstream pipelines.
- If you want, I can create the git commit and push the branch for you.
