class WearableLog {
  final int id;
  final DateTime sleepStart;
  final DateTime sleepEnd;
  final String hypnogram;
  final int sleepScore;
  final String sleepDate;

  WearableLog({
    required this.id,
    required this.sleepStart,
    required this.sleepEnd,
    required this.hypnogram,
    required this.sleepScore,
    required this.sleepDate,
  });

  factory WearableLog.fromJson(Map<String, dynamic> json) {
    return WearableLog(
      id: json['id'],
      sleepStart: DateTime.parse(json['sleepStart']),
      sleepEnd: DateTime.parse(json['sleepEnd']),
      hypnogram: json['hypnogram'],
      sleepScore: json['sleepScore'],
      sleepDate: json['sleepDate'],
    );
  }
}
