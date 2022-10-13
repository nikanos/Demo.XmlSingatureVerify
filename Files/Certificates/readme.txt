# Create the self signed certificate with subject DEMO_XML_SIGNATURE_VERIFY_SIGN and KeySpec Signatue (default KeySpec value is None and we had issues with code that used the private key of the certificate)
# see https://learn.microsoft.com/en-us/powershell/module/pki/new-selfsignedcertificate?view=windowsserver2022-ps
$cert = New-SelfSignedCertificate -Subject DEMO_XML_SIGNATURE_VERIFY_SIGN -NotAfter (Get-Date).AddYears(10) -KeySpec Signature -Certstorelocation Cert:\CurrentUser\My

# Export the previously created certificate to PFX (including the private key) named DEMO_XML_SIGNATURE_VERIFY_SIGN.pfx in the current directory using the password qwerty
# see https://learn.microsoft.com/en-us/powershell/module/pki/export-pfxcertificate?view=windowsserver2022-ps
$mypwd = ConvertTo-SecureString -String "qwerty" -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath DEMO_XML_SIGNATURE_VERIFY_SIGN.pfx -Password $mypwd

# Export the previously created certificate to cer named DEMO_XML_SIGNATURE_VERIFY_SIGN.cer in the current directory
# see https://learn.microsoft.com/en-us/powershell/module/pki/export-certificate?view=windowsserver2022-ps
Export-Certificate -Cert $cert -FilePath DEMO_XML_SIGNATURE_VERIFY_SIGN.cer
