from flask import Flask,request,jsonify
import sqlite3 as sql

app=Flask(__name__)

@app.route("/")
def hello_world():
    return "<p>Hello, World!</p>"

@app.route("/get_ID",methods=["GET","POST"])
def get_ID():
    print("Working")
    if request.method=="POST":
        data=request.get_json()
        name=data.get("name")
        score=data.get("score")
        print(name)
        id=add_ID(name)
        update_score(id,score)
        return jsonify({"ID": id})
    
@app.route("/update_score",methods=["GET","POST"])
def update_score_route():
    if request.method=="POST":
        data=request.get_json()
        id=data.get("ID")
        score=data.get("score")
        update_score(id,score)
        return jsonify({"status":"success"})
    
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
cursor.close()
conn.close()
def check_ID(id):
    conn=sql.connect("SpaceRanger.db")
    cursor=conn.cursor()
    cursor=conn.cursor()
    cursor.execute("SELECT * FROM users WHERE ID=?", (id,))
    data=cursor.fetchone()
    cursor.close()
    if data:
        return True
    else:
        return False
    
def add_ID(name):
    conn=sql.connect("SpaceRanger.db")
    cursor=conn.cursor()
    cursor.execute("INSERT INTO users (name,score) VALUES (?,?)", (name,0,))
    conn.commit()
    id = cursor.lastrowid
    cursor.close()
    conn.close()
    return id
def update_score(id,score):
    conn=sql.connect("SpaceRanger.db")

    cursor=conn.cursor()
    cursor.execute("UPDATE users SET score=? WHERE ID=?", (score,id))
    conn.commit()
    cursor.close()
    conn.close()
def return_playerScores():
    conn=sql.connect("SpaceRanger.db")
    cursor=conn.cursor()
    cursor.execute("SELECT name,score FROM users ORDER BY score DESC")
    sorted_results = cursor.fetchall()
    cursor.close()
    conn.close()
    print("Sorted Results:", sorted_results)
    if sorted_results:
            scores_list = [{"playerName": row[0], "playerScore": row[1]} for row in sorted_results]
            return scores_list # JSON serializable
    else:
        return None
    
    
    