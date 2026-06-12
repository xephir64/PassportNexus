if (!([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) { Start-Process powershell.exe "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs; exit }

$root = New-SelfSignedCertificate `
    -Type Custom `
    -Subject "CN=Messenger Revival Root CA" `
    -KeyUsage CertSign,CRLSign,DigitalSignature `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -HashAlgorithm SHA256 `
    -KeyExportPolicy Exportable `
    -CertStoreLocation "Cert:\LocalMachine\My" `
    -NotAfter (Get-Date).AddYears(20) `
    -TextExtension @("2.5.29.19={critical}{text}CA=true")

Export-Certificate `
    -Cert $root `
    -FilePath C:\certs\MessengerRootCA.cer


$server = New-SelfSignedCertificate `
    -Type Custom `
    -DnsName "login.passport.com","nexus.passport.com","localhost","127.0.0.1" `
    -Signer $root `
    -KeyAlgorithm RSA `
    -KeyLength 2048 `
    -HashAlgorithm SHA256 `
    -KeyExportPolicy Exportable `
    -CertStoreLocation "Cert:\LocalMachine\My" `
    -NotAfter (Get-Date).AddYears(10)

$password = ConvertTo-SecureString `
    "msnpassport" `
    -Force `
    -AsPlainText

Export-PfxCertificate `
    -Cert $server `
    -FilePath C:\certs\passport.pfx `
    -Password $password