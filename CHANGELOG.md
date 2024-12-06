# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.13]

### Added

- Added support for handling build profiles in Unity 6 and above

## [1.0.12]

### Added

- Added handling CLI argument `-buildAppBundle` to produce an .abb file instead of .apk when building for Android

## [1.0.11]

### Added

- Added handling CLI arguments `-keystoreName`, `-keystorePass`, `-keystoreAliasName` and `-keystoreAliasPass` to perform signing for Android builds

## [1.0.10]

### Fixed

- Invalid minimum Unity version

## [1.0.9]

### Fixed

- Fixed argument constants

## [1.0.8]

### Added

- Add error log command line arguments not set properly

## [1.0.7]

### Added

- Add error log when build report result is not positive

## [1.0.6]

### Added

- Added first version of build script inspired by current Build V3 script

## [1.0.5]

### Changed

- Instead of specifying override paths, clear to null, which should automatically set the internal SDK paths

## [1.0.4]

### Added

- Add debug logs

## [1.0.3]

### Changed

- It appears the -executeMethod command line argument does not detect static entrypoints within a package assembly and requires the script to be in an Editor folder within the assets folder of the project. Implemented new approach to solve this problem

## [1.0.2]

### Fixed

- Fixed missing EditorApplication.Exit(0); call

## [1.0.1]

### Changed

- Instead of using [InitializeOnLoad] for initiazing Android settings, we'll do it via -executeMethod

## [1.0.0]

Initial release