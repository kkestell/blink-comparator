#!/usr/bin/env bash

dotnet publish --runtime linux-arm -p:PublishSingleFile=true --self-contained -c Release
scp -r ./BlinkComparator/bin/Release/net7.0/linux-arm/publish/blink-comparator ganymede:/home/$(whoami)/.local/bin/
