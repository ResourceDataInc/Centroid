#!/usr/bin/env python

from distutils.core import setup

# markdown isn't supported, so this'll be janky
with open('README.md') as file:
  long_description = file.read()

setup(name = 'centroid',
      version = '1.0.0',
      description = 'A centralized paradigm on configuration management.',
      long_description = long_description,
      author = 'Resource Data, Inc',
      author_email = 'oss@resdat.com',
      url = 'https://github.com/ResourceDataInc/Centroid',
      license = 'MIT',
      data_files = ['README.md'],
      py_modules = ['centroid'])
