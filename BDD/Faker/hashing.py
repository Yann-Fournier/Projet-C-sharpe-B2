import hashlib


def hash_string(password, salt, algorithm='sha1'):
    """Return a hashed password using the specified algorithm and salt."""
    hash = hashlib.new(algorithm, password + salt)
    print(hash.hexdigest())
    return '%s$%s$%s' % (algorithm, salt, hash.hexdigest())

