Welcome to ZSim. A fork of OpenSimulator.

# Overview

OpenSim is a BSD Licensed Open Source project to develop a functioning
virtual worlds server platform capable of supporting multiple clients
and servers in a heterogeneous grid structure. OpenSim is written in
C#, and can run under Mono or the Microsoft .NET runtimes.

This is considered an alpha release.  Some stuff works, a lot doesn't.
If it breaks, you get to keep *both* pieces.

# Compiling ZSim

Please see BUILDING.md if you downloaded a source distribution and 
need to build OpenSim before running it.

# Running ZSim on Windows

You will need .NET 4.0 for versions up to 0.9.0.1 and .NET 4.6 for others.

We recommend that you run OpenSim from a command prompt on Windows in order
to capture any errors.

To run ZSim from a command prompt

 * cd to the bin/ directory where you unpacked ZSim
 * review and change configuration files (.ini) for your needs. see the "Configuring ZSim" section
 * run zSim.exe or zsim32.exe for small regions


# Running ZSim on Linux

You will need Mono >= 2.10.8.1 up to version 0.9.0.1 and mono > 5.0 on others.  On some Linux distributions you
may need to install additional packages. Simply find the equivalent of mono-complete on your distribution and install it. You may need a basic code compiling toolchain, look it up for your distribution. On ubuntu this package is build-essential


To run ZSim, from the unpacked distribution type:

 * cd bin
 * review and change configuration files (.ini) for your needs. see the "Configuring ZSim" section
 * run ./zsim.sh


# Configuring ZSim

When ZSim starts for the first time, you will be prompted with a
series of questions that look something like:

	[09-17 03:54:40] DEFAULT REGION CONFIG: Simulator Name [ZSim Test]:

For all the options except simulator name, you can safely hit enter to accept
the default if you want to connect using a client on the same machine or over
your local network.

You will then be asked "Do you wish to join an existing estate?".  If you're
starting ZSim for the first time then answer no (which is the default) and
provide an estate name.

Shortly afterwards, you will then be asked to enter an estate owner first name,
last name, password and e-mail (which can be left blank).  Do not forget these
details, since initially only this account will be able to manage your region
in-world.  You can also use these details to perform your first login.

Once you are presented with a prompt that looks like:

	Region (My region name) #

You have successfully started ZSim.


# Connecting to your ZSim

By default your sim will be available for login on port 9000. 
You must use a viewer that supports OpenSim type grids.

To login to ZSim, use either the estate details, or just login with whatever credentials you want, by default, if the account does not exist it will be created. You can change this setting in Standalone.ini or in Robust.ini

# Bug reports

To file bug reports or feature requests see Bugs.md

# More Information on ZSim

Thanks for trying ZSim. We hope you like our patches more than the official OpenSim project!

