Framework "4.6"
properties {
	$BaseDir = Resolve-Path ".\src"
	$SolutionFile = "$BaseDir\Topshelf.Lamar.sln"
	$OutputDir1 = "$BaseDir\Topshelf.Lamar\bin"
	$OutputDir2 = "$BaseDir\Topshelf.Quartz.Lamar\bin"
	$Configuration = "Release"
}

task default -depends Build

task Init {
    cls
}

task Clean -depends Init {
    if (Test-Path $OutputDir1) {
        ri $OutputDir1 -Recurse
    }
	if (Test-Path $OutputDir2) {
        ri $OutputDir2 -Recurse
    }
}

task RestorePackages {
	exec { dotnet restore $solutionFile }
}

task Build -depends Init,Clean,RestorePackages {
    exec { dotnet build $SolutionFile --configuration $Configuration --no-restore --no-incremental }
}

task Publish -depends Build {
    exec {
        dotnet pack "$BaseDir\Topshelf.Lamar\Topshelf.Lamar.csproj" -o $OutputDir1 --no-build --include-symbols -c $Configuration
    }
	 exec {
        dotnet pack "$BaseDir\Topshelf.Quartz.Lamar\Topshelf.Quartz.Lamar.csproj" -o $OutputDir2 --no-build --include-symbols -c $Configuration
    }
}