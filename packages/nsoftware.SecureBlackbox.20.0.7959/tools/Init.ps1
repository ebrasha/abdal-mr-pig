param($installPath, $toolsPath, $package, $project)
$moniker = $project.Properties.Item("TargetFrameworkMoniker").Value
$framework = New-Object System.Runtime.Versioning.FrameworkName($moniker)

[System.Reflection.Assembly]::LoadFile("$($installPath)\lib\net20\nsoftware.SecureBlackbox.dll") | Out-Null

If ($framework.Identifier.Equals(".NETFramework")) {
  [nsoftware.SecureBlackbox.SecureBlackbox]::CheckLicense("nuget")
}Else{
  [nsoftware.SecureBlackbox.SecureBlackbox]::CheckLicense("nugetcore")
}
<#                      Identifer                       Version
  .NET Core             .NETCoreApp                     1.1
  .NET Standard 1.4     .NETStandard                    1.4
  .NET Standard 1.6     .NETStandard                    1.6
  UWP                   .NETCore                        5.0
  .NET Classic          .NETFramework                   4.5.2
  Xamarin.Androd        MonoAndroid                     6.0
  Xamarin.IOS           Xamarin.iOS                     1.0
#>



