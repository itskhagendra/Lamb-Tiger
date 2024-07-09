echo "Starting Build"
"C:\Program Files\Unity\Hub\Editor\2022.3.34f1\Editor\Unity.exe" -quit -batchmode -projectPath "D:\Unity Projects\Lamb Tiger" -executeMethod MultiPlatformBuild.PerformBuild -logFile build.log
echo "Build Completed"