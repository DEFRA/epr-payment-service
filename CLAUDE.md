# Agent Instructions — epr-payment-service

Human-facing setup is in [`README.md`](README.md). System-wide agent guidance lives in [DEFRA/epr-local-environment/agents/](https://github.com/DEFRA/epr-local-environment/tree/main/agents) (architecture, glossary, gotchas) — clone as a sibling of this repo if you haven't already; the agents docs assume that layout for cross-repo grep.

This repo owns `FeesPayment` (SQL Server). EF Core migrations live under `src/EPR.Payment.Service.Data/Migrations`.

