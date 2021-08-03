<a href="https://skyhop.org"><img src="https://skyhop.org/assets/images/skyhop.png" width=200 alt="skyhop logo" /></a>

----

Thank you for checking out the documentation for the `FlightAnalysis` project! This library is a powerful tool to process a flight track into information you can directly enter into your (EASA) flight log. The idea behind this library is that it should be possible to extract basic flight information based on automatic position reports broadcasted by systems like ADSB or Flarm. An alternative would be to manually extract the flight log from the avionic systems and analyse these later using a specific tool.

> This library is part of the foundation on which we run [SkyHop](https://skyhop.org), which is an automated digital logbook for general aviation.

## Features

The following features are currently available in the library:

* Post flight processing
* Real-time flight processing (based on live feeds)
* Tooling to help process information from either a single aircraft or a bunch of aircraft

## Platform Support

The compiled libraries are targeting .NET Standard 2. Therefore you can use these starting from .NET Framework 4.6.1.

## Installation

The library can be added to your project in a number of ways:

### NuGet

The `Skyhop.FlightAnalysis` library is available through NuGet and can be installed as follows:

    Install-Package Skyhop.FlightAnalysis

### .NET CLI

To install the library through the .NET CLI you can use the following command:

    dotnet add package Skyhop.FlightAnalysis

### Building From Source

You can always grab the source on [GitHub](https://github.com/SkyHop/FlightAnalysis) and build the code yourself!
