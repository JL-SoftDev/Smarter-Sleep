DROP TABLE IF EXISTS app_user CASCADE;
CREATE TABLE app_user (
    user_id UUID PRIMARY KEY,
    username VARCHAR(20) NOT NULL,
    created_at TIMESTAMP,
    points INT
);

DROP TABLE IF EXISTS custom_schedule CASCADE;
CREATE TABLE custom_schedule (
    user_id UUID,
    day_of_week INT,
    wake_time TIME,
    FOREIGN KEY (user_id) REFERENCES app_user(user_id) ON DELETE CASCADE,
    PRIMARY KEY (user_id, day_of_week)
);

DROP TABLE IF EXISTS wearable_data CASCADE;
CREATE TABLE wearable_data (
    id SERIAL PRIMARY KEY,
    sleep_start TIMESTAMP,
    sleep_end TIMESTAMP,
    hypnogram VARCHAR,
    sleep_score INT,
    sleep_date DATE NOT NULL
);

DROP TABLE IF EXISTS survey CASCADE;
CREATE TABLE survey (
    id SERIAL PRIMARY KEY,
    created_at TIMESTAMP NOT NULL,
    sleep_quality INT,
    wake_preference INT,
    temp_preference INT,
    lights_disturbance BOOLEAN,
    sleep_earlier BOOLEAN,
    ate_late BOOLEAN,
    sleep_duration INT,
    survey_date DATE NOT NULL
);


DROP TABLE IF EXISTS sleep_review;
CREATE TABLE sleep_review (
    id SERIAL PRIMARY KEY,
    user_id UUID NOT NULL,
    wearable_log_id INT,
    survey_id INT,
    created_at TIMESTAMP NOT NULL,
    smarter_sleep_score INT,
    FOREIGN KEY (user_id) REFERENCES app_user(user_id) ON DELETE CASCADE,
    FOREIGN KEY (wearable_log_id) REFERENCES wearable_data(id) ON DELETE CASCADE,
    FOREIGN KEY (survey_id) REFERENCES survey(id) ON DELETE CASCADE
);

DROP TABLE IF EXISTS sleep_settings CASCADE;
CREATE TABLE sleep_settings (
    id SERIAL PRIMARY KEY,
    user_id UUID NOT NULL,
    scheduled_sleep TIMESTAMP NOT NULL,
    scheduled_wake TIMESTAMP NOT NULL,
    scheduled_hypnogram VARCHAR,
    FOREIGN KEY (user_id) REFERENCES app_user(user_id) ON DELETE CASCADE
);

DROP TABLE IF EXISTS device CASCADE;
CREATE TABLE device (
    id SERIAL PRIMARY KEY,
    user_id UUID NOT NULL,
    name VARCHAR,
    type VARCHAR,
    ip VARCHAR,
    port INT,
    status VARCHAR,
    FOREIGN KEY (user_id) REFERENCES app_user(user_id) ON DELETE CASCADE
);

DROP TABLE IF EXISTS device_settings;
CREATE TABLE device_settings (
    id SERIAL PRIMARY KEY,
    device_id INT NOT NULL,
    sleep_settings_id INT NOT NULL,
    scheduled_time TIMESTAMP NOT NULL,
    settings JSONB,
    user_modified BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (device_id) REFERENCES device(id) ON DELETE CASCADE,
    FOREIGN KEY (sleep_settings_id) REFERENCES sleep_settings(id) ON DELETE CASCADE
);

DROP TABLE IF EXISTS daily_streak;
CREATE TABLE daily_streak (
    user_id UUID PRIMARY KEY,
    start_date DATE NOT NULL,
    last_date DATE NOT NULL,
    FOREIGN KEY (user_id) REFERENCES app_user(user_id) ON DELETE CASCADE
);

DROP TABLE IF EXISTS item CASCADE;
CREATE TABLE item (
    id SERIAL PRIMARY KEY,
    name VARCHAR NOT NULL UNIQUE,
    description TEXT,
    cost INT NOT NULL
);

DROP TABLE IF EXISTS challenge CASCADE;
CREATE TABLE challenge (
    id INT PRIMARY KEY,
    name VARCHAR NOT NULL UNIQUE,
    description TEXT,
    reward INT NOT NULL
);

DROP TABLE IF EXISTS user_challenge CASCADE;
CREATE TABLE user_challenge (
    id SERIAL PRIMARY KEY,
    user_id UUID NOT NULL,
    challenge_id INT NOT NULL,
    completed BOOLEAN DEFAULT FALSE,
    start_date TIMESTAMP,
    expire_date TIMESTAMP,
    user_selected BOOLEAN NOT NULL, 
    FOREIGN KEY (user_id) REFERENCES app_user(user_id) ON DELETE CASCADE,
    FOREIGN KEY (challenge_id) REFERENCES challenge(id) ON DELETE CASCADE,
    CONSTRAINT user_unique_challenge UNIQUE(user_id, challenge_id)
);

DROP TABLE IF EXISTS transaction CASCADE;
CREATE TABLE transaction (
    id SERIAL PRIMARY KEY,
    user_id UUID NOT NULL,
    created_at TIMESTAMP,
    point_amount INT NOT NULL,
    description VARCHAR,
    FOREIGN KEY (user_id) REFERENCES app_user(user_id) ON DELETE CASCADE
);

DROP TABLE IF EXISTS purchase_log;
CREATE TABLE purchase_log (
    id SERIAL PRIMARY KEY,
    item_id INT NOT NULL,
    transaction_id INT NOT NULL,
    FOREIGN KEY (item_id) REFERENCES item(id),
    FOREIGN KEY (transaction_id) REFERENCES transaction(id) ON DELETE CASCADE
);

DROP TABLE IF EXISTS challenge_log;
CREATE TABLE challenge_log (
    id SERIAL PRIMARY KEY,
    challenge_id INT NOT NULL,
    transaction_id INT NOT NULL,
    FOREIGN KEY (challenge_id) REFERENCES challenge(id) ON DELETE CASCADE,
    FOREIGN KEY (transaction_id) REFERENCES transaction(id) ON DELETE CASCADE
);