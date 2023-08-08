[![.NET](https://github.com/aimenux/CertificateCli/actions/workflows/ci.yml/badge.svg)](https://github.com/aimenux/CertificateCli/actions/workflows/ci.yml)

# CertificateCli
```
A global tool to generate rsa/ecc certificates and json web keys
```

> In this repo, i m building a global tool that allows to generate rsa/ecc certificates and json web keys.
>
> The tool is based on two sub commands :
> - Use sub command `Rsa` to generate an [rsa certificate](https://sectigostore.com/page/what-is-an-rsa-certificate/)
> - Use sub command `Ecc` to generate an [ecc certificate](https://sectigostore.com/page/what-is-an-ecc-ssl-certificate/)
> - Use sub command `Jwk` to generate a [json web key](https://datatracker.ietf.org/doc/html/rfc7517)
>
>
> To run the tool, type commands :
> - `CertificateCli -h` to show help
> - `CertificateCli -v` to show version
> - `CertificateCli -s` to show settings
> - `CertificateCli Rsa -f [certificate-file] -p [certificate-password]` to generate rsa certificate
> - `CertificateCli Ecc -f [certificate-file] -p [certificate-password]` to generate ecc certificate
> - `CertificateCli Jwk -f [certificate-file] -p [certificate-password]` to generate json web key
>
>
> To install global tool from a local source path, type commands :
> - `dotnet tool install -g --configfile .\nugets\local.config CertificateCli --version "*-*" --ignore-failed-sources`
>
> To install global tool from [nuget source](https://www.nuget.org/packages/JwtCli), type these command :
> - For stable version : `dotnet tool install -g CertificateCli --ignore-failed-sources`
> - For prerelease version : `dotnet tool install -g CertificateCli --version "*-*" --ignore-failed-sources`
>
> To uninstall global tool, type these command :
> - `dotnet tool uninstall -g CertificateCli`
>
>
> ![CertificateCli](Screenshots/CertificateCli.png)
>

**`Tools`** : vs22, net 6.0/7.0, certificate-manager, text-copy, command-line, spectre-console