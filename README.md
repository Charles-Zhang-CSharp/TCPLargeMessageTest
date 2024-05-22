# TCP Large Message Test

Status: In-progress
Estimated effort: 20 hours

This setup tests what happens to TCP when VERY large packages are sent - because I remember from Unreal times there are talks on network limit on 512 bytes of data size, and based on my practical experience with Godot, it cannot receive large WebSocket messages reliable - is this fundamentally because of how TCP works? That's what this setup is inteded to test out.

Specifically, we test:
1. Whether it's possible to send very large packages in TCP at all and how complicated is the API/protocol.
2. Any difference in behavior between text-based message and binary data.
3. What exactly is this "data order" protection mechanism?
4. How fast/efficient is sending data this way, especially when compared to UDP.
5. Any other related concerns.