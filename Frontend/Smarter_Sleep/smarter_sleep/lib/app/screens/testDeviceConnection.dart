import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

import '../appFrame.dart';

class TestDeviceConnection extends StatefulWidget{
  const TestDeviceConnection({super.key});
  
  @override
  State<StatefulWidget> createState() => testingDeviceConnectionState();
}

class Album {
  final int userId;
  final int id;
  final String title;

  const Album({
    required this.userId,
    required this.id,
    required this.title,
  });

  factory Album.fromJson(Map<String, dynamic> json) {
    return Album(
      userId: json['userId'],
      id: json['id'],
      title: json['title'],
    );
  }
}
class testingDeviceConnectionState extends State<TestDeviceConnection>{
  late Future<Album> futureAlbum;

  @override
  void initState() {
    super.initState();
    futureAlbum = fetchAlbum();
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
      bottomNavigationBar: Padding(
        padding: const EdgeInsets.all(16.0),
        
        ),
    );

    throw UnimplementedError();
  }
void _addTemperatureDevice(){

}

void _addSoundDevice(){

}

void _addLightDevice(){

}
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
}