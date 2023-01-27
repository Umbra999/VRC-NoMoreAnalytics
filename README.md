# VRC-NoMoreAnalytics
Melonloader Mod to Prevent VRChat Analytics
I use custom deobfuscation Maps and a custom injection Method, i know most people use EAC Melon but the Maps should still work, if they don't fix it yourself

# How can this help
This is preventing all VRChat Analytics to not get send to them, this makes ban evasion easier and also prevents your alt Accounts from getting banned if one Account gets banned

# Further Steps
i strongly recommend using this with Knahs universal unity HWID spoofer, don't use some monkey shit, they most likely don't know what they are doing

# What is this doing
![image](https://user-images.githubusercontent.com/69671761/214994195-e52537f6-e40c-45da-8891-f6fcdaef542b.png)

This boi right here gets initializing a new Instance of the AmplitudeWrapper so we already prevent it at this step and in every method i found with xref too just to be sure


![image](https://user-images.githubusercontent.com/69671761/214994370-d58ed2fa-5396-4d4d-bc4c-5ed3611916bb.png)

Now we have most of this methods bypassed and all outgoing reqs would fail

![image](https://user-images.githubusercontent.com/69671761/214994415-16c47c93-45b3-46c4-b80a-adc2251c73ba.png)

Just to be sure we stop the Loop too incase they try to add a check and reinit it here at some point (will also give you some of your sweet frames back)
