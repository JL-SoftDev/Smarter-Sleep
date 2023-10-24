import 'package:flutter/material.dart';

import 'testDeviceConnection.dart';

class DeviceConnectionsScreen extends StatefulWidget {
  const DeviceConnectionsScreen({super.key});

  @override
  State<DeviceConnectionsScreen> createState() =>
      _DeviceConnectionsScreenState();
}

class _DeviceConnectionsScreenState extends State<DeviceConnectionsScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: Column(children: [
      Flexible(
          child: ListView.separated(
              itemCount: 10,
              separatorBuilder: (context, index) {
                return const SizedBox(height: 16);
              },
              itemBuilder: (context, index) {
                return Container(
                    padding: const EdgeInsets.symmetric(horizontal: 16),
                    height: 64,
                    child: Row(
                      children: [
                        //Icon
                        Container(
                            padding: const EdgeInsets.all(8),
                            height: 48,
                            width: 48,
                            color: Colors.grey.shade300,
                            child: const Icon(Icons.lightbulb)),
                        //Spacing
                        const SizedBox(width: 24),
                        //Name
                        const Text("Device Name", style: TextStyle(fontSize: 18))
                      ],
                    ));
              })),
      //Bottom button
      TextButton(
          onPressed: _navigateToDeviceConnectionScreen, child: const Text("Connect a Device"))
    ]));
  }

  void _connectADeviceClicked() {}

  void _navigateToDeviceConnectionScreen(){
     Navigator.push(
        context, MaterialPageRoute(builder: (context) => const TestDeviceConnection()));
  }
}
