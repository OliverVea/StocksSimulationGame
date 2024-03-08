## Installation

> Note: The following instructions are for Windows. If you are using another operating system, you can find the instructions for it in the [official documentation](https://nodejs.org/en/download/package-manager/).

### Installing NVM for Windows

To install Node.js, you need to install NVM for Windows. Run the following command in PowerShell as an administrator:

```bash
iex (new-object net.webclient).downloadstring('https://raw.githubusercontent.com/creationix/nvm/v1.1.7/install.ps1')
```

### Installing Node.js

To install Node.js, run the following command in PowerShell:

```bash
nvm install lts
```

To use the installed version of Node.js, run the following command in PowerShell:

```bash
nvm use lts
```

### Installing Choco

To install the required packages, you need to install Choco. Run the following command in PowerShell as an administrator:

```bash
Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))
```

### Creating certificates

To run the server in development mode, you need to create a local CA and a local certificate for the server.

To create a local CA and a local certificate for the server, you can use `mkcert`. Install it globally using the following command:

```bash
choco install mkcert
```

Run the following command to install the local CA:

```bash
mkcert -install
```

Create a `certs` directory in the root of the project and navigate to it:

```bash
mkdir certs && cd certs
```

```bash
mkcert -key-file ./certs/localhost-key.pem -cert-file ./certs/localhost.pem localhost 127.0.0.1 ::1
```

This will create a `certs` directory with the `localhost-key.pem` and `localhost.pem` files.

### Running the server

To run the server in development mode, use the following command:

```bash
npm run dev
```

### Generating the client code

To generate the client code, run the following command in PowerShell:

```bash
npm run generate
```
