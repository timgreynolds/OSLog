#!/bin/zsh

echo "##### Starting generation of iOS Simulator library #####"
sleep 2
xcodebuild -project OSLogNative.xcodeproj -scheme OSLogNative.ios -destination 'platform=iOS Simulator,name=iPhone 12' -destination 'generic/platform=iOS' -configuration Debug -derivedDataPath "${PWD}"

echo "##### Starting generation of iOS Phone library #####"
sleep 2
xcodebuild -project OSLogNative.xcodeproj -scheme OSLogNative.ios -destination 'generic/platform=iOS' -configuration Debug -derivedDataPath "${PWD}"

echo "##### Starting generation of Mac Catalyst library #####"
sleep 2
xcodebuild -target OSLogNative.maccatalyst SYMROOT="${PWD}/Build/Products"

echo "##### Starting generation of MacOS library #####"
sleep 2
xcodebuild -target OSLogNative.macos SYMROOT="${PWD}/Build/Products"

echo "##### All libraries generated; creating iOS fat library #####"
sleep 2
if [[ -f "${PWD}/Build/Products/Debug-iphoneos/libOSLogNative.ios.dylib" && -f "${PWD}/Build/Products/Debug-iphonesimulator/libOSLogNative.ios.dylib" ]]; then
  pushd "${PWD}/Build/Products"
  lipo -create "./Debug-iphoneos/libOSLogNative.ios.dylib" "./Debug-iphonesimulator/libOSLogNative.ios.dylib" -output "./Debug/libOSLogNative.ios.dylib"
  echo "Fat library created at '${PWD}/Debug/libOSLogNative.ios.dylib'"
  popd
else
  echo "One or more of the required build products is missing."
fi
