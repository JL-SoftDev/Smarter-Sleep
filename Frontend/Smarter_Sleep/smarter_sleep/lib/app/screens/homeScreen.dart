import 'package:flutter/material.dart';

import '../appFrame.dart';

class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Home"),
        actions: [
          IconButton(
            icon: const Icon(Icons.account_circle),
            onPressed: () {
              mainNavigatorKey.currentState!.pushNamed("/account");
            },
          ),
        ],
      ),
      body: SafeArea(
          child: Column(children: [
        //Top Buttons
        Row(children: [
          //Devices and Alarms Button
          TextButton(
              onPressed: _devicesAndAlarmClicked,
              child: Text("Devices & Alarms")),
          //Spacer
          Spacer(),
          //Sleep Button
          TextButton(
              onPressed: _sleepButtonClicked, child: Text("Sleep Button"))
        ]),
        //Spacer
        Spacer(),
        //Middle Content
        Align(
            alignment: Alignment.centerLeft,
            child: Container(
                color: Colors.grey.shade300, height: 200, width: 150)),
        //Bottom Padding
        SizedBox(height: 120)
      ])),
    );
  }

  void _devicesAndAlarmClicked() {
    mainNavigatorKey.currentState!.pushNamed("/devices");
  }

  void _sleepButtonClicked() {}
}
