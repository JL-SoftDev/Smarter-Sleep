import 'package:flutter/material.dart';

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
              itemCount: 32,
              separatorBuilder: (context, index) {
                return SizedBox(height: 16);
              },
              itemBuilder: (context, index) {
                return Container(
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    height: 64,
                    child: Row(
                      children: [
                        //Icon
                        Container(
                            padding: EdgeInsets.all(8),
                            height: 48,
                            width: 48,
                            color: Colors.grey.shade300,
                            child: Icon(Icons.lightbulb)),
                        //Spacing
                        SizedBox(width: 24),
                        //Name
                        Text("Device Name", style: TextStyle(fontSize: 18))
                      ],
                    ));
              })),
      //Bottom button
      TextButton(
          onPressed: _connectADeviceClicked, child: Text("Connect a Device"))
    ]));
  }

  void _connectADeviceClicked() {}
}
