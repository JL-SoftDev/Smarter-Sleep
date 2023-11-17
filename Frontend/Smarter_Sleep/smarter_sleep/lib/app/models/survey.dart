class Survey {
  final int id;
  final DateTime createdAt;
  final int sleepQuality;
  final int wakePreference;
  final int temperaturePreference;
  final bool lightsDisturbance;
  final bool sleepEarlier;
  final int sleepDuration;
  final String surveyDate;

  Survey({
    required this.id,
    required this.createdAt,
    required this.sleepQuality,
    required this.wakePreference,
    required this.temperaturePreference,
    required this.lightsDisturbance,
    required this.sleepEarlier,
    required this.sleepDuration,
    required this.surveyDate,
  });

  factory Survey.fromJson(Map<String, dynamic> json) {
    return Survey(
      id: json['id'],
      createdAt: DateTime.parse(json['createdAt']),
      sleepQuality: json['sleepQuality'],
      wakePreference: json['wakePreference'],
      temperaturePreference: json['temperaturePreference'],
      lightsDisturbance: json['lightsDisturbance'],
      sleepEarlier: json['sleepEarlier'],
      sleepDuration: json['sleepDuration'],
      surveyDate: json['surveyDate'],
    );
  }
}
