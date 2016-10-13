#!/usr/bin/env python

from distutils.core import setup

setup(name = 'centroid',
      version = '1.2.0',
      description = 'A centralized paradigm to configuration management.',
      long_description = 'Centroid is a tool for loading configuration values declared in JSON, and accessing those configuration values using object properties.',
      author = 'Resource Data, Inc',
      author_email = 'oss@resdat.com',
      url = 'https://github.com/ResourceDataInc/Centroid',
      license = 'MIT',
      data_files = ['README.md'],
      py_modules = ['centroid'])
