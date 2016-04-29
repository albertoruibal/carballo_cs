using System;
using System.IO;
using System.Text;
using Com.Alonsoruibal.Chess;
using Com.Alonsoruibal.Chess.Book;
using Com.Alonsoruibal.Chess.Log;
using Com.Alonsoruibal.Chess.Search;
using Sharpen;

namespace Com.Alonsoruibal.Chess.Uci
{
	/// <summary>UCI Interface</summary>
	public class Uci : SearchObserver
	{
		internal const string Name = "Carballo Chess Engine v1.5";

		internal const string Author = "Alberto Alonso Ruibal";

		internal Config config;

		internal SearchEngineThreaded engine;

		internal SearchParameters searchParameters;

		internal bool needsReload = true;

		public Uci ()
		{
			Logger.noLog = true;
			// Disable logging
			config = new Config ();
			config.SetBook (new ResourceBook ("Carballo.book_small.bin"));
		}

		internal virtual void Loop ()
		{
			System.Console.Out.WriteLine (Name + " by " + Author);
			try {
				while (true) {
					string @in = Console.ReadLine ();
					string[] tokens = @in.Split (" ");
					int index = 0;
					string command = tokens [index++].ToLower ();
					if ("uci".Equals (command)) {
						System.Console.Out.WriteLine ("id name " + Name);
						System.Console.Out.WriteLine ("id author " + Author);
						System.Console.Out.WriteLine ("option name Hash type spin default " + Config.DefaultTranspositionTableSize + " min 16 max 256");
						System.Console.Out.WriteLine ("option name Ponder type check default " + Config.DefaultPonder);
						System.Console.Out.WriteLine ("option name OwnBook type check default " + Config.DefaultUseBook);
						System.Console.Out.WriteLine ("option name UCI_Chess960 type check default false");
						System.Console.Out.WriteLine ("option name Evaluator type combo default " + Config.DefaultEvaluator + " var simplified var complete var experimental");
						System.Console.Out.WriteLine ("option name Elo type spin default " + Config.DefaultElo + " min 500 max " + Config.DefaultElo);
						System.Console.Out.WriteLine ("uciok");
					} else {
						if ("setoption".Equals (command)) {
							index++;
							// Skip name
							// get the option name without spaces
							StringBuilder nameSB = new StringBuilder ();
							string tok;
							while (!"value".Equals (tok = tokens [index++])) {
								nameSB.Append (tok);
							}
							string name = nameSB.ToString ();
							string value = tokens [index++];
							if ("Hash".Equals (name)) {
								config.SetTranspositionTableSize (System.Convert.ToInt32 (value));
							} else if ("Ponder".Equals (name)) {
								config.SetPonder (System.Boolean.Parse (value));
							} else if ("OwnBook".Equals (name)) {
								config.SetUseBook (System.Boolean.Parse (value));
							} else if ("UCI_Chess960".Equals (name)) {
								config.SetUciChess960 (System.Boolean.Parse (value));
							} else if ("Elo".Equals (name)) {
								config.SetElo (System.Convert.ToInt32 (value));
							}
							needsReload = true;
						} else {
							if ("isready".Equals (command)) {
								if (needsReload) {
									engine = new SearchEngineThreaded (config);
									engine.SetObserver (this);
									needsReload = false;
									System.GC.Collect ();
								} else {
									// Wait for the engine to finish searching
									while (engine.IsSearching ()) {
										try {
											Sharpen.Thread.Sleep (10);
										} catch (Exception) {
										}
									}
								}
								System.Console.Out.WriteLine ("readyok");
							} else {
								if ("quit".Equals (command)) {
									System.Environment.Exit (0);
								} else {
									if ("go".Equals (command)) {
										searchParameters = new SearchParameters ();
										while (index < tokens.Length) {
											string arg1 = tokens [index++];
											if ("searchmoves".Equals (arg1)) {
											} else {
												// TODO
												if ("ponder".Equals (arg1)) {
													searchParameters.SetPonder (true);
												} else {
													if ("wtime".Equals (arg1)) {
														searchParameters.SetWtime (System.Convert.ToInt32 (tokens [index++]));
													} else {
														if ("btime".Equals (arg1)) {
															searchParameters.SetBtime (System.Convert.ToInt32 (tokens [index++]));
														} else {
															if ("winc".Equals (arg1)) {
																searchParameters.SetWinc (System.Convert.ToInt32 (tokens [index++]));
															} else {
																if ("binc".Equals (arg1)) {
																	searchParameters.SetBinc (System.Convert.ToInt32 (tokens [index++]));
																} else {
																	if ("movestogo".Equals (arg1)) {
																		searchParameters.SetMovesToGo (System.Convert.ToInt32 (tokens [index++]));
																	} else {
																		if ("depth".Equals (arg1)) {
																			searchParameters.SetDepth (System.Convert.ToInt32 (tokens [index++]));
																		} else {
																			if ("nodes".Equals (arg1)) {
																				searchParameters.SetNodes (System.Convert.ToInt32 (tokens [index++]));
																			} else {
																				if ("mate".Equals (arg1)) {
																					searchParameters.SetMate (System.Convert.ToInt32 (tokens [index++]));
																				} else {
																					if ("movetime".Equals (arg1)) {
																						searchParameters.SetMoveTime (System.Convert.ToInt32 (tokens [index++]));
																					} else {
																						if ("infinite".Equals (arg1)) {
																							searchParameters.SetInfinite (true);
																						}
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
										engine.Go (searchParameters);
									} else {
										if ("stop".Equals (command)) {
											engine.Stop ();
										} else {
											if ("ucinewgame".Equals (command)) {
												engine.GetBoard ().StartPosition ();
												engine.Clear ();
											} else {
												if ("position".Equals (command)) {
													if (index < tokens.Length) {
														string arg1 = tokens [index++];
														if ("startpos".Equals (arg1)) {
															engine.GetBoard ().StartPosition ();
														} else {
															if ("fen".Equals (arg1)) {
																// FEN string may have spaces
																StringBuilder fenSb = new StringBuilder ();
																while (index < tokens.Length) {
																	if ("moves".Equals (tokens [index])) {
																		break;
																	}
																	fenSb.Append (tokens [index++]);
																	if (index < tokens.Length) {
																		fenSb.Append (" ");
																	}
																}
																engine.GetBoard ().SetFen (fenSb.ToString ());
															}
														}
													}
													if (index < tokens.Length) {
														string arg1 = tokens [index++];
														if ("moves".Equals (arg1)) {
															while (index < tokens.Length) {
																int move = Move.GetFromString (engine.GetBoard (), tokens [index++], true);
																engine.GetBoard ().DoMove (move);
															}
														}
													}
												} else {
													if ("debug".Equals (command)) {
													} else {
														if ("ponderhit".Equals (command)) {
															if (searchParameters != null) {
																searchParameters.SetPonder (false);
																engine.SetSearchLimits (searchParameters, false);
															}
														} else {
															if ("register".Equals (command)) {
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			} catch (IOException e) {
				// not used
				// System.out.println("Command not recognized: " + in);
				Sharpen.Runtime.PrintStackTrace (e);
			}
		}

		public virtual void BestMove (int bestMove, int ponder)
		{
			StringBuilder sb = new StringBuilder ();
			sb.Append ("bestmove ");
			sb.Append (Move.ToString (bestMove));
			if (config.GetPonder () && ponder != Move.None) {
				sb.Append (" ponder ");
				sb.Append (Move.ToString (ponder));
			}
			System.Console.Out.WriteLine (sb.ToString ());
			System.Console.Out.Flush ();
		}

		public virtual void Info (SearchStatusInfo info)
		{
			System.Console.Out.Write ("info ");
			System.Console.Out.WriteLine (info.ToString ());
		}

		public static void Main (string[] args)
		{
			Com.Alonsoruibal.Chess.Uci.Uci uci = new Com.Alonsoruibal.Chess.Uci.Uci ();
			uci.Loop ();
		}
	}
}
