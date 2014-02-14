require "bundler/setup"
require "albacore"

desc "Build everything"
build :build do |cmd|
  cmd.sln = "dot-net/Centroid.sln"
end

namespace :package do
  directory "dot-net/build/pkg"
  desc "Package .NET"
  nugets_pack :cs => ["dot-net/build/pkg", :test] do |cmd|
    cmd.exe = "dot-net/build/support/NuGet.exe"
    cmd.out = "dot-net/build/pkg"
    cmd.files = ["dot-net/Centroid/Centroid.csproj"]
    cmd.with_metadata do |m|
      m.description = "Centralized configuration management."
      m.authors = "Resource Data, Inc."
      m.version = "1.0.0"
      m.license_url = "https://github.com/ResourceDataInc/Centroid/blob/master/LICENSE.txt"
      m.project_url = "https://github.com/ResourceDataInc/Centroid"
    end
    cmd.gen_symbols
  end

  desc "Package Python"
  task :py do
    puts `cd python && python setup.py sdist`
  end
end

namespace :release do
  desc "Release .NET package"
  task :cs  => ["package:cs"] do
    Dir.glob("dot-net/build/pkg/*.nupkg") do |f|
      system "dot-net/build/support/NuGet.exe", ["push", f], :clr_command => true
    end
  end

  desc "Release Python package"
  task :py do
    exec "cd python && python setup.py sdist upload"
  end
end

namespace :test do
  desc "Test .NET"
  test_runner :cs => [:build] do |cmd|
    cmd.exe = "dot-net/packages/NUnit.Runners.2.6.3/tools/nunit-console.exe"
    cmd.files = ["dot-net/Centroid.Tests/bin/Debug/Centroid.Tests.dll"]
  end

  desc "Test python"
  task :py do
    puts `python -m unittest python.tests`
  end
end

desc "Test everything"
task :test => ["test:cs", "test:py"]

task :default => :test
