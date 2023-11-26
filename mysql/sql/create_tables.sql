USE group_ordering;

CREATE TABLE IF NOT EXISTS test (
  id        INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  test_name VARCHAR(10) NOT NULL,
  age       INT NOT NULL
);

INSERT INTO test (test_name, age) VALUE('test', 23);