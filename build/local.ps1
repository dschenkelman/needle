#Directory structure to be used
#
#  \build          - This is where the project build code lives
#  \BuildOutput - This folder is created if it is missing and contains output of the build
#  \src           - This folder contains the solution
#
Properties {
	$build_dir = Split-Path $psake.build_script_file	
	$build_output_dir = "$build_dir\..\buildOutput\"
	$code_dir = "$build_dir\..\src"
    $vstest = "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe"
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends Nuget, Test

Task Nuget -Depends Build {
    Exec { .\Nuget.exe Pack needle.nuspec -OutputDirectory $build_output_dir}
}

Task Test -Depends Build {
    Assert(Test-Path($vstest)) "vstest.console.exe must be available."
    $testAssemblies = Get-ChildItem $build_output_dir -recurse -include *Tests.dll
    Exec { & $vstest $testAssemblies /EnableCodeCoverage } 
}

Task Build -Depends Clean {	
	Write-Host "Building NeedleContainer.sln" -ForegroundColor Green
	Exec { msbuild "$code_dir\NeedleContainer.sln" /t:Build /p:Configuration=Release /v:quiet /p:OutDir=$build_output_dir } 
}

Task Clean {
	Write-Host "Creating BuildOutput directory" -ForegroundColor Green
	if (Test-Path $build_output_dir) 
	{	
		rd $build_output_dir -rec -force | out-null
	}
	
	mkdir $build_output_dir | out-null
	
	Write-Host "Cleaning NeedleContainer.sln" -ForegroundColor Green
	Exec { msbuild "$code_dir\NeedleContainer.sln" /t:Clean /p:Configuration=Release /v:quiet } 
}