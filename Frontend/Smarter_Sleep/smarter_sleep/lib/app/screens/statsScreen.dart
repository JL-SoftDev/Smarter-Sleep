import 'package:flutter/material.dart';

class StatsScreen extends StatefulWidget {
  const StatsScreen({super.key});

  @override
  State<StatsScreen> createState() => _StatsScreenState();
}

class _StatsScreenState extends State<StatsScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: SafeArea(
            child: Column(children: [
      //Top Section
      Container(
        height: MediaQuery.of(context).size.height * 0.40,
        width: double.infinity,
        color: Colors.grey.shade700,
        child: const Column(
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
        child: const Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
          Text("Last REM: XX.XX"),
        ]),
      ))
    ])));
  }
}
