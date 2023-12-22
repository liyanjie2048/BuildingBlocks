setlocal enabledelayedexpansion

for /R "dist" %%i in (*.nupkg) do (
	echo %%i
	dotnet nuget push "%%i" -s liyanjie2048
)
