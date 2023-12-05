import hashlib

def hash_passwd(password):
    return hashlib.sha256(password.encode('utf-8')).hexdigest()
