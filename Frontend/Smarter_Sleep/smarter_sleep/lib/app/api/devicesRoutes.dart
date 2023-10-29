import 'dart:convert';

import 'package:smarter_sleep/app/models/device.dart';
import 'package:http/http.dart' as http;

class DeviceRoutes {
  /*
  Future<List<Device>> getDevices() async {
    final response = await http.get(Uri.parse(''));

    if (response.statusCode == 200) {
      // If the server did return a 200 OK response,
      // then parse the JSON.
      return Device.fromJson(jsonDecode(response.body) as Map<String, dynamic>);
    } else {
      // If the server did not return a 200 OK response,
      // then throw an exception.
      throw Exception('Failed to load device');
    }
  }
  */

  Future<Device> getDevice(int id) async {
    final response = await http.get(Uri.parse(''));

    if (response.statusCode == 200) {
      // If the server did return a 200 OK response,
      // then parse the JSON.
      return Device.fromJson(jsonDecode(response.body) as Map<String, dynamic>);
    } else {
      // If the server did not return a 200 OK response,
      // then throw an exception.
      throw Exception('Failed to load device');
    }
  }

  Future addDevice(Device device) async {
    //Convert our device to JSON format
    var httpContent = device.toJson();
    final response =
        await http.post(Uri.parse(''), body: jsonEncode(httpContent));

    if (response.statusCode != 200) {
      // If the server did not return a 200 OK response,
      // then throw an exception.
      throw Exception('Failed to add device');
    }
  }

  Future updateDevice(Device device) async {
    //Convert our device to JSON format
    var httpContent = device.toJson();
    final response =
        await http.put(Uri.parse(''), body: jsonEncode(httpContent));

    if (response.statusCode != 200) {
      // If the server did not return a 200 OK response,
      // then throw an exception.
      throw Exception('Failed to update device');
    }
  }

  Future deleteDevice(int id) async {
    final response = await http.delete(Uri.parse(''));

    if (response.statusCode != 200) {
      // If the server did not return a 200 OK response,
      // then throw an exception.
      throw Exception('Failed to delete device');
    }
  }
}
