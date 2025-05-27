#!/bin/bash

# Stop on errors
set -e

# Get the directory where the compiler lives
SCRIPTDIR=`dirname $0`
CCDIR=$SCRIPTDIR/../src/SwishCC

# The test suite doesn't know that it needs to invoke the "real" driver with "dotnet", so this
# is a little shim to take care of that.
dotnet run --project $CCDIR/SwishCC.csproj -- $*


