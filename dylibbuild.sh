#!/bin/zsh

echo "##### Starting generation of iOS library #####"
sleep 2
xcodebuild -project OSLogNative.xcodeproj -scheme OSLogNative.ios -destination 'generic/platform=iOS Simulator' -destination 'generic/platform=iOS' -configuration Debug SYMROOT="${PWD}/Build"

echo "##### Starting generation of Mac Catalyst library #####"
sleep 2
xcodebuild -project OSLogNative.xcodeproj -scheme OSLogNative.maccatalyst -destination "generic/platform=macOS,variant=Mac Catalyst" -configuration Debug SYMROOT="${PWD}/Build"

echo "##### Starting generation of MacOS library #####"
sleep 2
xcodebuild -project OSLogNative.xcodeproj -scheme OSLogNative.macos -destination "generic/platform=macOS" -configuration Debug SYMROOT="${PWD}/Build"
