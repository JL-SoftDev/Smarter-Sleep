import 'package:flutter/material.dart';
import '../appFrame.dart';

class StatsScreen extends StatefulWidget {
  const StatsScreen({super.key});

  @override
  State<StatsScreen> createState() => _StatsScreenState();
}

class _StatsScreenState extends State<StatsScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text("Sleep Statistics"),
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
          //Top Section
          Container(
            height: MediaQuery.of(context).size.height * 0.40,
            width: double.infinity,
            color: Colors.grey.shade700,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                Text("Avg Sleep Time: XX:XX"),
                Text("Highest Sleep Score: XXXXXX"),
                Text("Best Temperature: XX F"),
              ],
            ),
          ),
          //Bottom Section
          Expanded(
              child: Container(
            color: Colors.grey,
            width: double.infinity,
            child:
                Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
              Text("Last REM: XX.XX"),
            ]),
          ))
        ])));
  }
}
