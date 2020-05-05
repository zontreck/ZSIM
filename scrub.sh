#! /bin/bash

git clean -xfd
git submodule foreach --recursive git clean -xfd
git reset --hard
git submodule foreach --recursive git reset --hard
git submodule update --init --recursive --remote --force
git submodule foreach --recursive git checkout master
git submodule foreach --recursive git reset --hard
git submodule foreach --recursive git clean -xfd
git submodule foreach --recursive git pull
