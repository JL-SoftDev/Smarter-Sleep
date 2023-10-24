import 'package:flutter/material.dart';

import 'deviceConnectionScreen.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
          child: Column(children: [
        //Top Buttons
        Row(children: [
          //Devices and Alarms Button
          TextButton(
              onPressed: _devicesAndAlarmClicked,
              child: const Text("Devices & Alarms")),
          //Spacer
          const Spacer(),
          //Sleep Button
          TextButton(
              onPressed: _sleepButtonClicked, child: const Text("Sleep Button"))
        ]),
        //Spacer
        const Spacer(),
        //Middle Content
        Align(
            alignment: Alignment.centerLeft,
            child: Container(
                color: Colors.grey.shade300, height: 200, width: 150)),
        //Bottom Padding
        const SizedBox(height: 120)
      ])),
    );
  }

  void _devicesAndAlarmClicked() {
    Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => const DeviceConnectionsScreen()));
  }

  void _sleepButtonClicked() {}
}
