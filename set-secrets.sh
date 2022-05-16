#!/bin/bash
cat secrets.json | dotnet user-secrets set -p Kino
