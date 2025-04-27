from flask import Flask,request,jsonify
import sqlite3 as sql

app=Flask(__name__)

@app.route("/")
def hello_world():
    return "<p>Hello, World!</p>"

@app.route("/get_ID",methods=["GET","POST"])
def get_ID():
    if request.method=="POST":
        print("Working")
        data=request.get_json()

        name=data.get("name")
        print(request.form)
        id=add_ID(name)
        return jsonify({"ID":id})
@app.route("/update_score",methods=["GET","POST"])
def update_score_route():
    if request.method=="POST":
        id=request.form["ID"]
        score=request.form["score"]
        if check_ID(id):
            update_score(id,score)
@app.route("/get_scores")
def get_scores():
    scores=return_playerScores()
    if scores:
        return jsonify({"scores":scores})
    else:
        return jsonify({"scores":None})


#Database code
conn=sql.connect("SpaceRanger.db")
cursor=conn.cursor()

cursor.execute('''
CREATE TABLE IF NOT EXISTS users (
    ID INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    score INTEGER NOT NULL 
)
''')

def check_ID(id):
    cursor.execute("SELECT * FROM users WHERE ID=?", (id,))
    data=cursor.fetchone()
    if data:
        return True
    else:
        return False
def add_ID(name):
    cursor.execute("INSERT INTO users (name) VALUES (?)", (name,))
    conn.commit()
    id = cursor.lastrowid
    return id
def update_score(id,score):
    cursor.execute("UPDATE users SET score=? WHERE ID=?", (score,id))
    conn.commit()
def return_playerScores():
    cursor.execute("SELECT * FROM users ORDER BY score DESC")
    sorted_results = cursor.fetchall()
    if sorted_results:
        return sorted_results
    else:
        return None
    