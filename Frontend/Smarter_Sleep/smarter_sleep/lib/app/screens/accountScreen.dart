import 'package:flutter/material.dart';
import 'package:amplify_flutter/amplify_flutter.dart';

import '../appFrame.dart';

class AccountScreen extends StatefulWidget {
  const AccountScreen({super.key});

  @override
  State<AccountScreen> createState() => _AccountScreenState();
}

class _AccountScreenState extends State<AccountScreen> {
  Map<String, String> userAttributes = {};

  @override
  void initState() {
    super.initState();
    _fetchUserAttributes();
  }

  Future<void> _fetchUserAttributes() async {
    try {
      final result = await Amplify.Auth.fetchUserAttributes();
      final Map<String, String> attributes = {
        for (var attribute in result)
          attribute.userAttributeKey.key: attribute.value
      };

      setState(() {
        userAttributes = attributes;
      });
    } catch (e) {
      safePrint('Error fetching user attributes: $e');
    }
  }

  Future<void> _signOut() async {
    try {
      await Amplify.Auth.signOut();
    } catch (e) {
      safePrint('Error signing out: $e');
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('User Account Information'),
      ),
      body: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: <Widget>[
          Expanded(
            child: ListView(
              children: userAttributes.keys.map((key) {
                return ListTile(
                  title: Text(key),
                  subtitle: Text('${userAttributes[key]}'),
                );
              }).toList(),
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: Row(
              children: [
                TextButton(
                  onPressed: () {
                    _signOut();
                  },
                  style: ButtonStyle(
                    foregroundColor:
                        MaterialStateProperty.all<Color>(Colors.red),
                  ),
                  child: const Text('Logout', style: TextStyle(fontSize: 18.0)),
                ),
                const SizedBox(width: 16.0),
                TextButton(
                  onPressed: () {
                    mainNavigatorKey.currentState!.pushNamed("/debug");
                  },
                  style: ButtonStyle(
                    foregroundColor:
                        MaterialStateProperty.all<Color>(Colors.blue),
                  ),
                  child: const Text('Tester Debug',
                      style: TextStyle(fontSize: 18.0)),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}
