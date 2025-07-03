#!/bin/bash
# Kill process running on port 5000
port=5000
pid=$(lsof -i :$port -t)

if [ -n "$pid" ]; then
    kill -9 $pid
    echo "Process on port $port killed."
else
    echo "No process found on port $port."
fi
