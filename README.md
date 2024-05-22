# TCP Large Message Test

Status: In-progress

This setup tests what happens to TCP when VERY large packages are sent - because I remember from Unreal times there are talks on network limit on 512 bytes of data size, and based on my practical experience with Godot, it cannot receive large WebSocket messages reliable - is this fundamentally because of how TCP works? That's what this setup is inteded to test out.