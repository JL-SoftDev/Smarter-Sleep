import 'dart:convert';
import 'package:amplify_flutter/amplify_flutter.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:smarter_sleep/app/api/devicesRoutes.dart';

import '../appFrame.dart';
import '../models/device.dart';

class TestDeviceConnection extends StatefulWidget {
  const TestDeviceConnection({super.key});

  @override
  State<StatefulWidget> createState() => testingDeviceConnectionState();
}

class testingDeviceConnectionState extends State<TestDeviceConnection> {
  final List<DropdownMenuEntry<String>> _deviceTypes = [
    const DropdownMenuEntry<String>(value: "light", label: "Light"),
    const DropdownMenuEntry<String>(value: "alarm", label: "Alarm"),
    const DropdownMenuEntry<String>(value: "thermostat", label: "Thermostat"),
  ];

  ///Handles the device route API calls
  DeviceRoutes _deviceRoutes = DeviceRoutes();

  ///Handles text input controller for device name
  TextEditingController _textEditingController = TextEditingController();

  ///Device type
  String _deviceType = "";

  ///User ID for ampliy user
  String? _userID = "";

  @override
  void initState() {
    super.initState();
    _getUserID();
    //Set default value
    _deviceType = _deviceTypes[0].value;
  }

  void _getUserID() async {
    final result = await Amplify.Auth.fetchUserAttributes();
    final Map<String, String> attributes = {
      for (var attribute in result)
        attribute.userAttributeKey.key: attribute.value
    };
    _userID = attributes["sub"];
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Connect Device"),
        actions: [
          IconButton(
            icon: const Icon(Icons.account_circle),
            onPressed: () {
              mainNavigatorKey.currentState!.pushNamed("/account");
            },
          ),
        ],
      ),
      body: ListView(
        children: [
          //Input field
          const Text("Device Name"),
          TextFormField(controller: _textEditingController),
          //Spacing
          const SizedBox(height: 24),
          //Type field
          const Text("Device Name"),
          DropdownMenu<String>(
              initialSelection: _deviceTypes[0].value,
              onSelected: _onDeviceTypeSelected,
              dropdownMenuEntries: _deviceTypes),
          //Add device button
          TextButton(
            onPressed: _addDevice,
            child: const Text("Add Device"),
          )
        ],
      ),
      bottomNavigationBar: const Padding(
        padding: EdgeInsets.all(16.0),
      ),
    );

    throw UnimplementedError();
  }

  void _addTemperatureDevice() {}

  void _addSoundDevice() {}

  void _addLightDevice() {}
  /*
  Future<Album> fetchAlbum() async {
    final response = await http
        .get(Uri.parse('http://ec2-54-87-139-255.compute-1.amazonaws.com/'));

    if (response.statusCode == 200) {
      // If the server did return a 200 OK response,
      // then parse the JSON.
      return Album.fromJson(jsonDecode(response.body));
    } else {
      // If the server did not return a 200 OK response,
      // then throw an exception.
      throw Exception('Failed to load album');
    }
  }
  */

  void _onDeviceTypeSelected(String? value) {
    _deviceType = value ?? "";
  }

  void _addDevice() async {
    /*
      class Device {
      int id = 0;
      String name = "";
      String type = "";
      String ip = "";
      int port = 0;
      String status = "";

      Device();

      factory Device.fromJson(Map<String, dynamic> json) => _$DeviceFromJson(json);
      Map<String, dynamic> toJson() => _$DeviceToJson(this);
    */

    //Create new device from device information
    var newDevice = Device();
    newDevice.userId = _userID == null ? "" : _userID!;
    newDevice.name = _textEditingController.text;
    newDevice.type = _deviceType;
    try {
      await _deviceRoutes.addDevice(newDevice);
      Navigator.pop(context);
    } on Exception catch (_) {
      //Indicate some type of error
    }
  }
}
