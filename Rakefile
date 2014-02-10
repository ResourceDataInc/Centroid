require "bundler/setup"
require "albacore"

desc "Build everything"
build :build do |cmd|
  cmd.sln = "dot-net/Centroid.sln"
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